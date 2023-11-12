﻿using WebApplication2.Models.Enum;

namespace WebApplication2.ViewModels
{
    public class CreateQuestionViewModel
    {
        public string question { get; set; }
        public int gradingType { get; set; }
        public string? comentary { get; set; }
        public string[]? answerVariants { get; set; } // ListAnsw and null
    }
}
