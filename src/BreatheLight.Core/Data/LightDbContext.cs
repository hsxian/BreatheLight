using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BreatheLight.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BreatheLight.Core.Data
{
    public class LightDbContext : DbContext
    {
        public LightDbContext() { }
        public LightDbContext(DbContextOptions<LightDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitSample(modelBuilder);
            modelBuilder.Entity<LightSequence>()
                .HasIndex(b => b.Id)
                .IsUnique();
        }
        private void InitSample(ModelBuilder modelBuilder)
        {
            var lists = new List<LightSequence>()
            {
                new LightSequence
                {
                    Id = 1,
                    StartTime = new DateTime(1, 1, 1, 6, 30, 0),
                    EndTime = new DateTime(1, 1, 1, 7, 0, 0),
                    BrightnessA = 2,
                    BrightnessB = 100,
                    BrightnessStep = 0.4f
                },
                new LightSequence
                {
                    Id = 2,
                    StartTime = new DateTime(1, 1, 1, 7, 30, 0),
                    EndTime = new DateTime(1, 1, 1, 8, 0, 0),
                    BrightnessA = 100,
                    BrightnessB = 2,
                    BrightnessStep = 0.4f
                }
            };
            modelBuilder.Entity<LightSequence>().HasData(lists);
            modelBuilder.Entity<LightStatus>().HasData(new LightStatus
            {
                Id = "-1",
                Brightness = 9,
            });
        }
        public DbSet<LightStatus> LightStatuses { get; set; }
        public DbSet<LightSequence> LightSequences { get; set; }
    }
}