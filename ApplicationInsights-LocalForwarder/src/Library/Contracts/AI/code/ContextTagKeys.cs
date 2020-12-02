namespace Microsoft.LocalForwarder.Library.Inputs.Contracts
{
    public class ContextTagKeys
    {
        public string ApplicationVersion { get; set; } = "ai.application.ver";

        public string DeviceId { get; set; } = "ai.device.id";

        public string DeviceLocale { get; set; } = "ai.device.locale";

        public string DeviceModel { get; set; } = "ai.device.model";

        public string DeviceOEMName { get; set; } = "ai.device.oemName";

        public string DeviceOSVersion { get; set; } = "ai.device.osVersion";

        public string DeviceType { get; set; } = "ai.device.type";

        public string LocationIp { get; set; } = "ai.location.ip";

        public string OperationId { get; set; } = "ai.operation.id";

        public string OperationName { get; set; } = "ai.operation.name";

        public string OperationParentId { get; set; } = "ai.operation.parentId";

        public string OperationSyntheticSource { get; set; } = "ai.operation.syntheticSource";

        public string OperationCorrelationVector { get; set; } = "ai.operation.correlationVector";

        public string SessionId { get; set; } = "ai.session.id";

        public string SessionIsFirst { get; set; } = "ai.session.isFirst";

        public string UserAccountId { get; set; } = "ai.user.accountId";

        public string UserAgent { get; set; } = "ai.user.userAgent";

        public string UserId { get; set; } = "ai.user.id";

        public string UserAuthUserId { get; set; } = "ai.user.authUserId";

        public string CloudRole { get; set; } = "ai.cloud.role";

        public string CloudRoleInstance { get; set; } = "ai.cloud.roleInstance";

        public string InternalSdkVersion { get; set; } = "ai.internal.sdkVersion";

        public string InternalAgentVersion { get; set; } = "ai.internal.agentVersion";

        public string InternalNodeName { get; set; } = "ai.internal.nodeName";
    }
}
