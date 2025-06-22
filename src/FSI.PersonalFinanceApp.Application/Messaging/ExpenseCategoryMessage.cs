using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public class ExpenseCategoryMessage
    {
        public string Action { get; set; } // "Create", "Update", "Delete"
        public ExpenseCategoryDto Payload { get; set; }
    }
}
