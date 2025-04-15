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

            descriptor.Field(x => x.Name)
                .Type<NonNullType<StringType>>()
                .Description("The name of the product.");

            descriptor.Field(x => x.Price)
                .Type<NonNullType<DecimalType>>()
                .Description("The price of the product.");
        }
    }
}


//Feature                  InputObjectType<T>(Mutation Input)              ObjectType<T>(Query Output)
//Used for	               Sending data to mutations	                     Returning data in queries
//Allows resolvers	       ❌ No	                                         ✅ Yes
//Allows computed fields   ❌ No	                                         ✅ Yes
//Used in	               Mutation arguments	                             Query & Mutation responses
//Example	               ProductInputType	                                 ProductType