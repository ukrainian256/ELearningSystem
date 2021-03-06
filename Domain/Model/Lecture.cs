﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain
{
    public class Lecture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public Guid ID { get; set; }

        public Guid TopicId { get; set; }

        public virtual CourseTopic Topic { get; set; }

        [Required]
        public string Name { get; set; }

        public string Homework { get; set; }

        [Required]
        public string LectureContent { get; set; }

        public decimal OrderNumber { get; set; }
    }
}
