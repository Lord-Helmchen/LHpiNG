namespace LHpiNG.Album
{
    public enum Rarity
    {
        None        = 0b000000000, //  0
        Common      = 0b000000001, //  1
        Uncommon    = 0b000000010, //  2
        Rare        = 0b000000100, //  4
        Mythic      = 0b000001000, //  8
        BasicLand   = 0b000010000, // 16
        Token       = 0b000100000, // 32
        Special     = 0b001000000, // 64
        Other       = 0b010000000, //128
    }
}