namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Sets I to the location of the sprite for the character in VX.Characters 0-F(in hexadecimal) are represented by a 4x5 font/
    /// </summary>
    public class xFX29 : BaseCommand
    {
        int X;

        public xFX29(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            I = (ushort)(V[X] * 0x5);
            PC += 2;
        }
    }
}
