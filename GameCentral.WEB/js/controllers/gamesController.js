var app = angular.module("games-app")

app.controller("gamesController", function ($scope, $http){
    
    $scope.Page = 'Home_Page'
    
    $scope.Games = []
    
    $scope.GameAdd = {}
    
    $scope.ViewGame = {}
    
    $scope.GameForEdit = {}
        
    $scope.ShowGames = function (reload){
        if(reload){
            $scope.LoadGames()
        }
        $scope.ViewGame = {}
        $scope.GameAdd = {}
        document.title = 'List of games'
        $scope.Page = 'ShowGames_Page'
    }
    
    $scope.About = function (){
        document.title = 'About'
        $scope.Page = 'About_Page'
    }
    
    $scope.GoHome = function () {
        document.title = 'GameCentral'
        $scope.Page = 'Home_Page'
    }
    
    $scope.AddGame = function (){
        document.title = 'Add game'
        $scope.Page = 'Add_Page'
    }
    
    $scope.ShowError = function(){
        document.title = 'Error'
        $scope.Page = 'Error_Page'
    }
    
    $scope.ViewGameDetails = function(gameId, gameTitle){
        document.title = gameTitle
        $scope.GetGame(gameId)
        $scope.Page = 'View_Page'
    }
    
    $scope.DeleteGame = function(gameId){
        let url = 'https://localhost:5002/api/games/' + gameId
        $http.delete(url)
            .then(function (data) {
                if (data.status === 200){
                    let temp = []
                    $scope.Games.forEach(function (game) {
                        if (game.gameId !== gameId){
                            temp.push(game)
                        }
                    })
                    $scope.Games = temp;
                }
            }, function (error) {
                alert('Error during delete game: [' + url + ']')
            })
    }
    
    $scope.ShowEditGame = function(id, title){
        $scope.GameForEdit = {
            gameId: id,
            title: title
        }
        document.title = title
        $scope.Page = 'Edit_Page'
    }
    
    $scope.SubmitEditGame = function(){
        let url = 'https://localhost:5002/api/games/' + $scope.GameForEdit.gameId
        $http.put(url, $scope.GameForEdit)
            .then(function (response) {
                if(response.status === 200){
                    let index = 0
                    for (let i = 0; i < $scope.length; i++){
                        if ($scope.Games[i].gameId === $scope.GameForEdit.gameId){
                            index = i;
                            break
                        }
                    }
                    $scope.Games[index] = $scope.GameForEdit
                    $scope.ShowGames(false);
                }
            }, function (error) {
                alert('Error during edit game: [' + url + ']')
            })
    }
    
    $scope.SubmitAddForm = function () {
        let url = 'https://localhost:5002/api/games'
        $http.post(url, $scope.GameAdd)
            .then(function Success(data) {
                if(data.status){
                    $scope.Games.push($scope.GameAdd)
                }
                $scope.ShowGames(false)
            }, function Error(e) {
                alert('Error during add game: [' + url + ']')
            })
    }
    
    $scope.LoadGames = function (){
        let url = 'https://localhost:5002/api/games'
        $http.get(url)
            .then(function Success(result) {
                $scope.Games = result.data
            }, function Error(error) {
                alert('Error during load games [' + url + ']')
            })
    }
    
    $scope.GetGame = function (gameId) {
        let req = 'https://localhost:5002/api/games/' + gameId
        $http.get(req)
            .then(function Success(result) {
                $scope.ViewGame = result.data
            }, function Error(error) {
                alert('Error during GET game [' + req + ']')
            })
    }
})
