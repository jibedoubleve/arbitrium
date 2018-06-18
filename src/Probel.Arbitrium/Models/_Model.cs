using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Probel.Arbitrium.Models
{
    public class Model
    {
        #region Properties

        [Key]
        [HiddenInput]
        public long Id { get; set; }

        #endregion Properties
    }
}