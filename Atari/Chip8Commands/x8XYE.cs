namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift
    /// </summary>
    public class x8XYE : BaseCommand
    {
        int X;

        public x8XYE(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            V[0xF] = (byte)(V[X] >> 7);
            V[X] <<= 1;
            PC += 2;
        }
    }
}
