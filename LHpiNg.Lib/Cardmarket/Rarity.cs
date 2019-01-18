namespace LHpiNG.Cardmarket
{
    public  enum Rarity
    {
        Common          = 0b00000000000, //   0
        Uncommon        = 0b00000000001, //   1
        Rare            = 0b00000000010, //   2
        Mythic          = 0b00000000100, //   4
        None            = 0b00000001000, //   8
        Land            = 0b00000010000, //  16
        Masterpiece     = 0b00000100000, //  32
        Special         = 0b00001000000, //  64
        TimeShifted     = 0b00010000000, // 128
        Token           = 0b00100000000, // 256
        ArenaCodeCard   = 0b01000000000, // 512
        TipCard         = 0b10000000000  //1024
    }
}