﻿namespace WebApplication2.Models.API
{
    public class Questionnaire
    {
        public int oid {  get; set; }
        public string name { get; set; }    
        //public List<Question> questions { get; set; } 
        public int companyOid { get; set; }
        public string company {  get; set; }
    }
}
