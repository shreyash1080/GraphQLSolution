using Application.Models;

namespace API.GraphQl.Types.InputType
{
    public class UpdateProductInputType : InputObjectType<UpdateProductModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductModel> descriptor)
        {
            descriptor.Name("UpdateProductInput");

            descriptor.Field(t => t.Id)
                .ID("Id")
                .Description("The ID of the product to update.")
                .Type<NonNullType<IdType>>();

            descriptor.Field(t => t.Name)
                .Description("The name of the product.")
                .Type<StringType>();

            descriptor.Field(t => t.Price)
                .Description("The price of the product.")
                .Type<DecimalType>();

            descriptor.Field(t => t.Description)
                .Description("The description of the product.")
                .Type<StringType>();

            descriptor.Field(t => t.Stock)
                .Description("The stock quantity of the product.")
                .Type<IntType>();

            descriptor.Field(t => t.IsAvailable)
                .Description("Indicates if the product is available.")
                .Type<BooleanType>();

            descriptor.Field(t => t.Category)
                .Description("The category of the product.")
                .Type<StringType>();

            descriptor.Field(t => t.SkuID)
                .Description("The SKU ID of the product.")
                .Type<StringType>();

            descriptor.Field(t => t.Supplier)
                .Description("The supplier of the product.")
                .Type<StringType>();

            descriptor.Field(t => t.Discount)
                .Description("The discount applied to the product.")
                .Type<DecimalType>();

        }
    }
}
