using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Features.Sync.Views
{
    [Table("SYNC_VIEW_ROUTES_USERS")]
    public class SyncViewRouteUser : EntityBase
    {
        [Key]
        [Column("ROUTE_ID")]
        public long Id { get; set; }

        [Column("SOURCE_NODE_ID")]
        public long SourceNodeId { get; set; }

        [Column("SOURCE_NODE_NAME")]
        public string SourceNodeName { get; set; } = string.Empty;

        [Column("SOURCE_NODE_URL")]
        public string SourceNodeUrl { get; set; } = string.Empty;

        [Column("TARGET_NODE_ID")]
        public long TargetNodeId { get; set; }

        [Column("TARGET_NODE_NAME")]
        public string TargetNodeName { get; set; } = string.Empty;

        [Column("TARGET_NODE_URL")]
        public string TargetNodeUrl { get; set; } = string.Empty;

        [Column("USER_NAME")]
        public string UserName { get; set; } = string.Empty;

        [Column("USER_IS_ACTIVE")]
        public bool UserIsActive { get; set; }

        [Column("ROUTE_IS_ACTIVE")]
        public bool RouteIsActive { get; set; }

        [Column("ROUTE_CREATED_AT")]
        public DateTime RouteCreatedAt { get; set; }

        [Column("ROUTE_UPDATED_AT")]
        public DateTime? RouteUpdatedAt { get; set; }
    }
}
