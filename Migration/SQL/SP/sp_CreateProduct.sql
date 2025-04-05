IF OBJECT_ID('sp_CreateProduct', 'P') IS NOT NULL  
    DROP PROCEDURE sp_CreateProduct;  
GO  

CREATE PROCEDURE sp_CreateProduct  
    @Name NVARCHAR(255),  
    @Price DECIMAL(18,2)  
AS  
BEGIN  
    SET NOCOUNT ON;  

    INSERT INTO Products (Name, Price)  
    OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Price  
    VALUES (@Name, @Price);  
END;  
GO  
