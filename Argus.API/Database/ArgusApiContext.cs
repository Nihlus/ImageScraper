//
//  ArgusApiContext.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using Argus.API.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Argus.API.Database;

/// <summary>
/// Represents the database context for the REST API.
/// </summary>
public class ArgusApiContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgusApiContext"/> class.
    /// </summary>
    /// <param name="options">The context options.</param>
    public ArgusApiContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the API keys in the database.
    /// </summary>
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKey>()
            .HasIndex(k => k.ID)
            .IsUnique();

        modelBuilder.Entity<ApiKey>()
            .HasIndex(k => k.Key)
            .IsUnique();
    }
}
