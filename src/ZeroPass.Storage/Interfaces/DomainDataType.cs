using System;

namespace ZeroPass.Storage
{
    [Flags]
    public enum DomainDataType : int
    {
        Domain                      = 0x0001,
        User                        = 0x0002,
        Group                       = 0x0004,
        ClientMachine               = 0x0008,
        ClientMachineStats          = 0x0010,
        VaultItem                   = 0x0020,
        VaultItemTag                = 0x0040,
        VaultItemUserAccess         = 0x0080,
        VaultItemStats              = 0x0100,
        VaultItemStarred            = 0x0200,
        DomainUser                  = 0x0400,
        GroupMemberShip             = 0x0800,
        VaultItemAccess             = 0x1000,
        // add combine value below
        GroupDetail = User | Group,
        VaultItemAccessUsers = User | VaultItemAccess,
        VaultItemListPersonal = VaultItem | VaultItemAccess,
        VaultItemListStarred = VaultItem | VaultItemAccess | VaultItemStarred,
        VaultItemDetail = VaultItem | VaultItemTag
    }
    
    public static class DataTypeExtensions
    {
        public static bool IsBasic(this DomainDataType t)
        {
            return ((t - 1) & t) == 0;
        }
    }
}