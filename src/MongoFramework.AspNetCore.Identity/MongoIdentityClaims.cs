using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MongoEntityFramework.AspNetCore.Identity
{
    /// <summary>
    /// Represents a claim that a user possesses.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for this user that possesses this claim.</typeparam>
    public class MongoIdentityUserClaim<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public virtual TKey User { get; set; } = default!;

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string? ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string? ClaimValue { get; set; }

        /// <summary>
        /// Converts the entity into a Claim instance.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType!, ClaimValue!);
        }

        /// <summary>
        /// Reads the type and value from the Claim.
        /// </summary>
        /// <param name="claim"></param>
        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }

    /// <summary>
    /// Represents a claim that is granted to all users within a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the role associated with this claim.</typeparam>
    public class MongoIdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {

        /// <summary>
        /// Gets or sets the of the primary key of the role associated with this claim.
        /// </summary>
        public virtual TKey Role { get; set; } = default!;

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string? ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string? ClaimValue { get; set; }

        /// <summary>
        /// Constructs a new claim with the type and value.
        /// </summary>
        /// <returns>The <see cref="Claim"/> that was produced.</returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType!, ClaimValue!);
        }

        /// <summary>
        /// Initializes by copying ClaimType and ClaimValue from the other claim.
        /// </summary>
        /// <param name="other">The claim to initialize from.</param>
        public virtual void InitializeFromClaim(Claim? other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }

}
