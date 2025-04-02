using Core.Entities;
using System.Xml.Linq;

namespace API.GraphQl.Types.InputType
{
    //ProductType is a custom GraphQL type that extends(:) ObjectType<Product>.
    //It tells GraphQL how to structure the Product entity when exposing it through queries or mutations.
    public class ProductType : ObjectType<Product> //ObjectType<T> is a built-in class from HotChocolate that helps map C# objects (Product) to GraphQL types.
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)//The Configure method is used to modify how GraphQL exposes the Product entity.
        {
            descriptor.Name("Product"); // Sets GraphQL type name to "Product"

            descriptor.Field(x => x.Name).Description("This is Product");

            descriptor.Field(x => x.Id).Description("The unique identifier of the product");

            descriptor.Field(x => x.Price).Description("This is price feild");
        }
    }
}

//What is IObjectTypeDescriptor<Product> ?
//descriptor is an instance of IObjectTypeDescriptor<Product>, which helps define how GraphQL should treat the Product class.
//It is used to:
//Rename fields(if needed).
//Add descriptions(helps API consumers).
//Hide fields(if needed).
//Define relationships with other entities.
