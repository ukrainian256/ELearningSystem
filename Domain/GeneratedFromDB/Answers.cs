//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.GeneratedFromDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answers
    {
        public System.Guid ID { get; set; }
        public System.Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsRight { get; set; }
    
        public virtual Questions Questions { get; set; }
    }
}