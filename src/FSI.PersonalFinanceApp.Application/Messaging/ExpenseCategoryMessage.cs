﻿using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public class ExpenseCategoryMessage
    {
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public ExpenseCategoryDto Payload { get; set; } = new();
        public long MessagingId { get; set; } // NOVO: Id da mensagem na tabela Messaging
    }
}
