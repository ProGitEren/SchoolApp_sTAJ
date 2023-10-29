
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using SchoolApplication.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApplication.Models
{


    public class Student : ApplicationUser
    {

        [Required]
        
        public string Department { get; set; }

        [Required]
        // [Range(250000,100000000000,ErrorMessage="FSDIHSDIHFLI")] is also okay but in my implementation this is fine     
        private decimal _price;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get { return _price; }
        internal set { _price = value; }
        }

        internal void SetPrice(decimal price) 
        {
            _price = price;
        }
        //public void UpdatePrice(decimal newprice) 
        //{
        //    Price = newprice;
        //}

        
        

        private int _discount;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public int Discount
        {
            get => _discount;
            internal set
            {
                _discount = value;
            }
        }

        [Required]

        private Dictionary<string, decimal> grades;

        public decimal Gpa  
        {
            get
            {
                decimal total = 0;
                int subjectCount = 0;

                foreach (string key in grades.Keys)
                {
                    if (grades[key] >= 0)
                    {
                        total += grades[key];
                        subjectCount++;
                    }
                }

                if (subjectCount > 0)
                {
                    decimal average = total / subjectCount;

                    return average;
                }


                return 0; // No valid subjects,
            }
            
        }

        public Dictionary<string,decimal> Grades { get { return grades; } }

        // [NotMapped]
        //public Dictionary<string, decimal> Grades 
        // {
        //     get { return grades; }
        //     set { grades = value; }

        // }
       public decimal Math { get;
            internal set;
        }
        public decimal Science
        {
            get;
            internal set;
        }
        public decimal Language
        {
            get;
            internal set;
        }
        public decimal History
        {
            get;
            internal set;
        }
        public decimal Sports
        {
            get;internal set;
        }


        [Required]
        public DateTime expiredate { get; set; }


        private void InitDictionary()
        {
            grades = new Dictionary<string, decimal>
            {
                {"Math",0 },
                {"Science",0},
                {"Language",0 },
                {"History",0 },
                {"Sports",0 }
            };
        }
        public Student()
        {
            InitDictionary();
        }

        public decimal Balance { get; set; } = 0;



    }
}
