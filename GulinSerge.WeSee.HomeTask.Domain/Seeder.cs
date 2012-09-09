using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	/// <summary>
	/// Генератор исходных данных для расчета.
	/// </summary>
	public interface Seeder
	{
		/// <summary>
		/// Подготовить исходные данные
		/// </summary>
		/// <param name="to">Правая граница вычислений</param>
		/// <returns>Простые числа для вычисления любых других простых чисел</returns>
		IEnumerable<ulong> PrepareSeed(ulong to);
	}
}
