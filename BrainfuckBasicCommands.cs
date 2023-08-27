using System;
using System.Text;

namespace func.brainfuck;

public class BrainfuckBasicCommands
{
	public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
	{
		RegisterCommandsWithOutput(vm, read, write);
		RegisterMemoryChangeCommands(vm);
		RegisterPointerShiftCommands(vm);
	}

	private static void RegisterCommandsWithOutput(IVirtualMachine vm, Func<int> read, Action<char> write)
	{
		vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
		vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
	}
	
	private static void RegisterMemoryChangeCommands(IVirtualMachine vm)
	{
		vm.RegisterCommand('+', b => b.Memory[b.MemoryPointer] = 
			(byte)((b.Memory[b.MemoryPointer] + 1)  % (byte.MaxValue + 1)));
		vm.RegisterCommand('-', b =>
		{
			if (b.Memory[b.MemoryPointer] > 0)
				b.Memory[b.MemoryPointer]--;
			else
				b.Memory[b.MemoryPointer] = byte.MaxValue;
		});
		var symbols = GetAllDigitsAndLetters();
		foreach (var symbol in symbols)
			vm.RegisterCommand(symbol, b => b.Memory[b.MemoryPointer] = (byte)symbol);
	}

	private static void RegisterPointerShiftCommands(IVirtualMachine vm)
	{
		vm.RegisterCommand('>', b => b.MemoryPointer = 
			(b.MemoryPointer + 1)  % (b.Memory.Length));
		vm.RegisterCommand('<', b =>
		{
			if (b.MemoryPointer > 0)
				b.MemoryPointer--;
			else
				b.MemoryPointer = b.Memory.Length - 1;
		});
	}

	private static string GetAllDigitsAndLetters()
	{
		var result = new StringBuilder();
		for (var symbol = '0'; symbol <= 'z'; symbol++)
		{
			if (char.IsLetter(symbol) || char.IsDigit(symbol))
				result.Append(symbol);
		}

		return result.ToString();
	}
}