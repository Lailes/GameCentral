var app = angular.module("games-app")

app.controller("gamesController", function ($scope, $http){
    
    $scope.Page = 'Home_Page'
    
    $scope.Games = []
    
    $scope.GameAdd = {}
    
    $scope.ViewGame = {}
    
    $scope.GameForEdit = {}
    
    $scope.LoadData = ''
    
    $scope.jwt = ''
    
    $scope.isAuth = false
    
    $scope.AuthData = {
        userName: '',
        password: ''
    }
    
    $scope.AddAdminData = {
        userName: '',
        password: ''
    }
    
    $scope.ShowAuth = function (){
        document.title = "Auth"
        $scope.Page = 'Auth_Page'
    }
    
    $scope.ShowAddAdmin = function (){
        document.title = 'Add admin'
        $scope.Page = 'AddAdmin_Page'
    }
    
    $scope.Auth = function (){
        let url = 'https://localhost:5002/api/auth/login'
        $http.post(url, $scope.AuthData).then(function Success(result){
            if (result.status === 200){
                $scope.jwt = result.data.token;
                $scope.isAuth = true;  
                $scope.GoHome();
            }
        }, function Error(result){
            if (result.status === 401){
                alert('Unauthorized')
            }else if (result.status === 403){
                alert('Incorrect username / password')
            } else{
                alert('Error during enter: [' + $scope.AuthData.userName + ']')
            }
        })
    }
    
    $scope.AddAdmin = function (){
        let url = 'https://localhost:5002/api/auth/register'
        
        $http.post(url, $scope.AddAdminData, {
            headers: {
                'Authorization': 'Bearer ' + $scope.jwt
            }
        }).then(function Success(result){
            if (result.status === 201){
                alert('User ' + $scope.AddAdminData.userName +  ' created')
                $scope.ShowGames(false);
            }else{
                alert('Unknown')
            }
        }, function Error(result){
            if (result.status === 401){
                alert('Unauthorized')
            }else if (result.status === 403){
                alert('Incorrect username / password')
            } else{
                alert('Error during enter: [' + $scope.AuthData.userName + ']')
            }
        })
    }
        
    $scope.ShowGames = function (reload){
        if(reload){
            let url = 'https://localhost:5002/api/games'
            $scope.LoadData = 'Loading games...'
            $scope.Page = 'Load_Page'
            $http.get(url)
                .then(function Success(result) {
                    $scope.Games = result.data
                    document.title = 'List of games'
                    $scope.Page = 'ShowGames_Page'
                    $scope.LoadData = ''
                }, function Error(error) {
                    alert('Error during load games [' + url + ']')
                })
        }
        $scope.ViewGame = {}
        $scope.GameAdd = {}
        $scope.GameForEdit = {}
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
        let req = 'https://localhost:5002/api/games/' + gameId
        $scope.Page = 'Load_Page'
        $scope.LoadData = 'Loading ' + gameTitle + '...'
        $http.get(req)
            .then(function Success(result) {
                $scope.ViewGame = result.data
                $scope.Page = 'View_Page'
                $scope.LoadData = ''
            }, function Error(error) {
                alert('Error during GET game [' + req + ']')
            })
    }
    
    $scope.DeleteGame = function(gameId){
        let url = 'https://localhost:5002/api/games/' + gameId
        $http.delete(url, {
            headers: {
                'Authorization': 'Bearer ' + $scope.jwt
            }
        }).then(function (data) {
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
                if (error.status === 401){
                    alert('Unauthorized')
                }else{
                    alert('Error during delete game: [' + url + ']')
                }
            })
    }
    
    $scope.ShowEditGame = function(game){
        $scope.GameForEdit = {
            gameId: game.gameId,
            title: game.title,
            genre: game.genre,
            cost: game.cost,
            publisher: game.publisher,
            studio: game.studio,
            description: game.description,
            previewImageUrl: game.previewImageUrl
        }
        document.title = game.title
        $scope.Page = 'Edit_Page'
    }
    
    $scope.SubmitEditGame = function(){
        let url = 'https://localhost:5002/api/games/' + $scope.GameForEdit.gameId
        $scope.LoadData = 'Loading data'
        $scope.Page = 'Load_Page'
        $http.put(url, $scope.GameForEdit, {
            headers: {
                'Authorization': 'Bearer ' + $scope.jwt
            }
        }).then(function (response) {
                if(response.status === 200){
                    let index = 0
                    for (let i = 0; i < $scope.length; i++){
                        if ($scope.Games[i].gameId === $scope.GameForEdit.gameId){
                            index = i;
                            break
                        }
                    }
                    $scope.Games[index] = $scope.GameForEdit
                    $scope.ShowGames(true);
                }
            }, function (error) {
                if (error.status === 401){
                    alert('Unauthorized')
                }else{
                    alert('Error during edit game: [' + url + ']')
                }
            })
    }
    
    $scope.SubmitAddForm = function () {
        let url = 'https://localhost:5002/api/games'
        $http.post(url, $scope.GameAdd, {
            headers: {
                'Authorization': 'Bearer ' + $scope.jwt
            }
        }).then(function Success(data) {
                $scope.ShowGames(true)
            }, function Error(e) {
                if (error.status === 401){
                    alert('Unauthorized')
                }else{
                    alert('Error during add game: [' + url + ']')
                }
            })
    }
})
