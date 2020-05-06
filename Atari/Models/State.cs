namespace Emulator
{
    public class State
    {
        public string GameName { get; set; }
        public byte[] Memory { get; set; }
        public ushort Timer_Sound { get; set; }
        public byte[] GFX { get; set; }
        public bool DrawFlag { get; set; }
        public ushort Opcode { get; set; }
        public byte[] V { get; set; }
        public ushort I { get; set; }
        public ushort PC { get; set; }
        public ushort Timer_Delay { get; set; }
        public ushort[] Stack { get; set; }
        public byte SP { get; set; }
    }
}
