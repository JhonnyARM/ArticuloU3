using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using cdcApi.Models;


namespace cdcApi.Data;

public partial class contextoContext : DbContext
{
  public contextoContext()
  {
  }

  public contextoContext(DbContextOptions<contextoContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Productos> products { get; set; }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseMySql(
        "Server=localhost;Port=3306;Database=inventory;User=debezium;Password=debezium;",
        ServerVersion.Parse("8.0.34-mysql")
    );
  }

}
