using API.GraphQl;
using API.GraphQL;
using Application.Models;
using HotChocolate.Types;

namespace API.GraphQl.Types.InputType
{
    //✅ InputObjectType<T> tells GraphQL that this is an input structure, not a database entity.  
    public class AddProductInputType : InputObjectType<ProductModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductModel> descriptor)
        {
            descriptor.Name("AddProductInput");

            descriptor.Field(t => t.Name)
                .Type<NonNullType<StringType>>()
                .Description("The name of the product.");

            descriptor.Field(t => t.Price)
                .Type<NonNullType<DecimalType>>()
                .Description("The price of the product.");

            descriptor.Field(t => t.CreatedAt)
                .Type<DateTimeType>()
                .Description("The creation date of the product.");

            descriptor.Field(t => t.Description)
                .Type<StringType>()
                .Description("The description of the product.");

            descriptor.Field(t => t.Stock)
                .Type<NonNullType<IntType>>()
                .Description("The stock quantity of the product.");

            descriptor.Field(t => t.IsAvailable)
                .Type<NonNullType<BooleanType>>()
                .Description("Indicates if the product is available.");

            descriptor.Field(t => t.Category)
                .Type<StringType>()
                .Description("The category of the product.");

            descriptor.Field(t => t.SkuID)
                .Type<StringType>()
                .Description("The Unique ID of the product.");

            descriptor.Field(t => t.Supplier)
                .Type<StringType>()
                .Description("The supplier of the product.");

            descriptor.Field(t => t.Discount)
                .Type<DecimalType>()
                .Description("The discount on the product.");

            descriptor.Field(t => t.ImageUrl)
                .Type<StringType>()
                .Description("The image URL of the product.");

            descriptor.Field(t => t.UserId)
                .Type<IntType>()
                .Description("The ID of the user associated with the product.");
         }
        
    }
 }


//Feature                  InputObjectType<T>(Mutation Input)              ObjectType<T>(Query Output)
//Used for	               Sending data to mutations	                     Returning data in queries
//Allows resolvers	       ❌ No	                                         ✅ Yes
//Allows computed fields   ❌ No	                                         ✅ Yes
//Used in	               Mutation arguments	                             Query & Mutation responses
//Example	               ProductInputType	                                 ProductType