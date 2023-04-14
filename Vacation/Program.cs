namespace Vacation
{
	class Program
	{
		static void Main(string[] args)
		{
			var vacationDictionary = new Dictionary<string, List<DateTime>>()
			{
				["Иванов Иван Иванович"] = new List<DateTime>(),
				["Петров Петр Петрович"] = new List<DateTime>(),
				["Юлина Юлия Юлиановна"] = new List<DateTime>(),
				["Сидоров Сидор Сидорович"] = new List<DateTime>(),
				["Павлов Павел Павлович"] = new List<DateTime>(),
				["Георгиев Георг Георгиевич"] = new List<DateTime>()
			};

			var workingDays = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }; //список рабочий дней
			var vacations = new List<DateTime>(); //общий список составленных дней отпуска для сотрудников

			foreach (var vacationList in vacationDictionary)
			{
				var random = new Random();
				var startDate = new DateTime(DateTime.Now.Year, 1, 1);
				var endDate = new DateTime(DateTime.Today.Year, 12, 31);
				var dateList = vacationList.Value; //список составленных дней отпуска для сотрудника

				//распределяем дни отпуска
				while (dateList.Count < 28) 
				{
					var range = (endDate - startDate).Days;
					var candidateStartDate = startDate.AddDays(random.Next(range)); //случайным образом выбираем начало для отпуска

					if (!workingDays.Contains(candidateStartDate.DayOfWeek))  //проверяем чтобы отпуск начаинался в будние дни
					{
						continue;
					}

					var vacationSteps = new[] { 7, 14 };
					var vacationStep = vacationSteps[random.Next(vacationSteps.Length)];  //случайным образом распределяем длину отпуска
					if (vacationStep + dateList.Count > 28)  //проверяем, чтобы не вышли за лимит в 28 дней
					{
						continue;
					}
					var candidateEndDate = candidateStartDate.AddDays(vacationStep); //конец планируемого отпуска
					if (!vacations.Any(dt => dt >= candidateStartDate && dt <= candidateEndDate) &&
						!vacations.Any(dt => dt.AddDays(3) >= candidateStartDate && dt.AddDays(3) <= candidateEndDate)) //проверяем не пересекается ли отпуск данного сотрудника с другими
					{
						var isVacationNotBe = dateList.Any(dt => dt.AddMonths(1) >= candidateStartDate &&
						dt.AddMonths(-1) <= candidateEndDate);                                                          //проверяем, чтобы отспуск не начинался за месяц до и месяц после уже утвержденных 

						if (isVacationNotBe)
						{
							continue;
						}
						else
						{
							for (DateTime dt = candidateStartDate; dt < candidateEndDate; dt = dt.AddDays(1))
							{
								vacations.Add(dt);
								dateList.Add(dt);
							}
						}
					}
				}
				Console.WriteLine($"Дни отпуска {vacationList.Key}:");
				Console.WriteLine(string.Join("\n", dateList));
			}
			Console.ReadKey();
		}
	}
}