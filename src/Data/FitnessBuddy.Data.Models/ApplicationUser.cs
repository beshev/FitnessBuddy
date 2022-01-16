﻿// ReSharper disable VirtualMemberCallInConstructor
namespace FitnessBuddy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using FitnessBuddy.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.Meals = new HashSet<Meal>();
            this.AddedFoods = new HashSet<Food>();
            this.FavoriteUserFoods = new HashSet<Food>();
        }

        public double WeightInKg { get; set; }

        public double GoalWeightInKg { get; set; }

        public string ProfilePicture { get; set; }

        public double HeightInCm { get; set; }

        public double DailyProteinGoal { get; set; }

        public double DailyCarbohydratesGoal { get; set; }

        public double DailyFatGoal { get; set; }

        public string AboutMe { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }

        [InverseProperty("AddedByUser")]
        public virtual ICollection<Food> AddedFoods { get; set; }

        public virtual ICollection<Food> FavoriteUserFoods { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
