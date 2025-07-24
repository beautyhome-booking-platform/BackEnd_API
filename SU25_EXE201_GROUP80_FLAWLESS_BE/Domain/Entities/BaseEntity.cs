using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Domain.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [BindNever]
        public Guid Id { get; set; }
        [BindNever]
        [JsonIgnore]
        public DateTime? CreateAt { get; set; }
        [BindNever]
        [JsonIgnore]
        public Guid? CreateBy { get; set; }
        [BindNever]
        [JsonIgnore]
        public DateTime? UpdateAt { get; set; }
        [BindNever]
        [JsonIgnore]
        public Guid? UpdateBy { get; set; }
        [BindNever]
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        [BindNever]
        [JsonIgnore]
        public DateTime? DeleteAt { get; set; }
        [BindNever]
        [JsonIgnore]
        public Guid? DeleteBy { get; set; }
    }
}
