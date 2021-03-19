﻿namespace NeoServer.Networking.Packets.Outgoing
{
    public enum GameOutgoingPacketType : byte
    {
        NoType = 0x00,
        Ping = 0x1E,
        SelfAppear = 0x0A,
        PlayerModes = 0xA7,
        Disconnect = 0x14,
        AddUnknownCreature = 0x61,
        AddKnownCreature = 0x62,
        MapDescription = 0x64,
        MapSliceNorth = 0x65,
        MapSliceEast = 0x66,
        MapSliceSouth = 0x67,
        MapSliceWest = 0x68,
        TileUpdate = 0x69,
        AddAtStackpos = 0x6A,
        TransformThing = 0x6B,
        RemoveAtStackpos = 0x6C,
        CreatureMoved = 0x6D,
        ContainerOpen = 0x6E,
        ContainerClose = 0x6F,
        ContainerAddItem = 0x70,
        ContainerUpdateItem = 0x71,
        ContainerRemoveItem = 0x72,
        InventoryItem = 0x78,
        InventoryEmpty = 0x79,
        OpenShop = 0x7A,
        SaleItemList = 0x7B,
        WorldLight = 0x82,
        MagicEffect = 0x83,
        AnimatedText = 0x84,
        DistanceShootEffect = 0x85,
        PlayerWalkCancel = 0xB5,
        CreatureLight = 0x8D,
        CreatureHealth = 0x8C,
        CreatureOutfit = 0x8E,
        CreatureSkull = 0x90,
        CreatureEmblem = 0x91,
        PlayerStatus = 0xA0,
        PlayerSkills = 0xA1,
        PlayerConditions = 0xA2,
        CreatureSpeech = 0xAA,
        CancelTarget = 0xA3,
        TextMessage = 0xB4,
        FloorChangeUp = 0xBE,
        FloorChangeDown = 0xBF,
        OutfitWindow = 0xC8,
        ReLoginWindow = 0x28,
        ChangeSpeed = 0x8F,
        OpenChannel = 0xAC,
        OpenPrivateChannel = 0xAD,
        SendPrivateMessage = 0xAA,
        ChannelList = 0xAB,
        CloseChannel = 0xB3,
        AddVip = 0xD2,
        OnlineStatusVip = 0xD3,
        OfflineStatusVip = 0xD4

    }
}
