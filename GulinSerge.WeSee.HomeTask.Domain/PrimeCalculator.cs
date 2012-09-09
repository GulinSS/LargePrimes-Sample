using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	/// <summary>
	/// Интерфейс-фасад над вычислениями
	/// </summary>
	public interface PrimeCalculator
	{
		IEnumerable<ulong> Calc(Task task);
	}
}
