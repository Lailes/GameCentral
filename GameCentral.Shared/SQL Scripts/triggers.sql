CREATE TRIGGER InsertGame ON GameCentral.Products.Games
    INSTEAD OF INSERT
    AS
BEGIN
    DECLARE @freeIndex INT;
    DECLARE @ind INT;
    SET @freeIndex = ISNULL((SELECT TOP 1 FreeIndex FROM Service.GameFreeIndexes), -1);

    IF @freeIndex = -1
        BEGIN
            SET @ind = (SELECT TOP 1 LastIndex FROM Service.GameLastIndex);
            UPDATE Service.GameLastIndex SET LastIndex = LastIndex + 1;
        END
    ELSE
        BEGIN
            DELETE FROM Service.GameFreeIndexes WHERE FreeIndex = @freeIndex;
            SET @ind = @freeIndex;
        END
    DECLARE @title NVARCHAR(128);
    DECLARE @desc NVARCHAR(MAX);
    DECLARE @stud NVARCHAR(64);
    DECLARE @genr NVARCHAR(32);
    DECLARE @publ NVARCHAR(64);
    DECLARE @cost INT;
    DECLARE @url NVARCHAR(128);
    SELECT TOP 1 @title = Title, @desc = Description, @stud = Studio, @genr = Genre, @publ = Publisher, @cost = Cost, @url = PreviewImageUrl  FROM inserted;
    INSERT INTO GameCentral.Products.Games VALUES (@ind, @title, @desc, @stud, @genr, @publ, @cost, @url);
end

GO;

CREATE TRIGGER DeleteGame ON GameCentral.Products.Games
    AFTER DELETE
    AS
    INSERT INTO GameCentral.Service.GameFreeIndexes VALUES ((SELECT GameId FROM deleted));
