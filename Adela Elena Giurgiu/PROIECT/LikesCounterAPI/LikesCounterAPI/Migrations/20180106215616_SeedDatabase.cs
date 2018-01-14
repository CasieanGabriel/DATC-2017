using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LikesCounterAPI.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Account (Id, Name, Likes) VALUES (1, 'Ion Popescu', 1000)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
