using Application.Models;

namespace API.GraphQl.Types.InputType
{
    public class AddUserInputType : InputObjectType<UserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserModel> descriptor)
        {
            descriptor.Field(u => u.Email).Description("The email address of the user.");
            descriptor.Field(u => u.Password).Description("The password of the user.");
            descriptor.Field(u => u.FirstName).Description("The first name of the user.");
            descriptor.Field(u => u.LastName).Description("The last name of the user.");
            descriptor.Field(u => u.CreatedAt).Description("The date and time when the user was created.");
            descriptor.Field(u => u.UpdatedAt).Description("The date and time when the user was last updated.");
        }
    }
}
