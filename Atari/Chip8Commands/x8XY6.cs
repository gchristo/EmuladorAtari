namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift
    /// </summary>
    public class x8XY6 : BaseCommand
    {
        int X;

        public x8XY6(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
        }

        public override void Execute()
        {
            V[0xF] = (byte)(V[X] & 0x1);
            V[X] >>= 1;
            PC += 2;
        }
    }
}
