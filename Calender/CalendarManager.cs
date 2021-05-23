using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CalendarManager : MonoBehaviour {

	///<summaryボタンの親オブジェクト</summary>
	public GameObject calendarParent;
	/// <summary>来月へ</summary>
	public Button nextButton;
	/// <summary>先月へ</summary>
	public Button prevButton;
	/// <summary>カレンダーの日時</summary>
	public DateTime current;
	/// <summary>Buttonオブジェクト</summary>
	GameObject[] objDays = new GameObject[42];
	/// <summary>カレンダーの日付マス</summary>
	CalendarButton[] Days = new CalendarButton[42];

	// Use this for initialization
	void Start () {
		current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

		InitCalendarComponent();
		SetCalendar();

		if(nextButton != null)
		{
			//押されたら起動
			nextButton.onClick.AsObservable()
				.Subscribe(_ =>
					{
						//一つ月を進める
						current = current.AddMonths(1);
						SetCalendar();
					});
		}
		if(prevButton != null)
		{
			prevButton.onClick.AsObservable()
				.Subscribe(_ =>
					{
						current = current.AddMonths(-1);
						SetCalendar();
					});
		}
	}

	/// <summary>コンポーネントの取得、設定</summary>
	void InitCalendarComponent()
	{
		//行
		for (int i = 0; i < calendarParent.transform.childCount; i++) {
			//子オブジェクトを保存
			objDays[i] = calendarParent.transform.GetChild(i).gameObject;
			//コンポーネントを設定、取得
			Days[i] = objDays[i].AddComponent<CalendarButton>();
			Days[i].index = i + 1;
		}
	}

	/// <summary>カレンダーに日付をセット</summary>
	void SetCalendar()
	{
		int day = 1;
		//今月の1日目
		var first = new DateTime(current.Year, current.Month, day);

		//来月
		var nextMonth = current.AddMonths(1);
		int nextMonthDay = 1;
		//先月
		var prevMonth = current.AddMonths(-1);
		//先月の場合は後ろから数える。
		int prevMonthDay = 
			DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month) - (int)first.DayOfWeek + 1;

		foreach (var cDay in Days)
		{
			//今月の1日より前のマスには先月の日にちを入れる。
			if(cDay.index <= (int)first.DayOfWeek)
			{
				cDay.dateValue = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDay);
				prevMonthDay++;
			}
			//今月の最終日より後ろのマスには来月の日にちを入れる。
			else if(day > DateTime.DaysInMonth(current.Year, current.Month))
			{
				cDay.dateValue = new DateTime(nextMonth.Year, nextMonth.Month, nextMonthDay);
				nextMonthDay++;
			}
			//今月の日にちをマスに入れる。
			else
			{
				cDay.dateValue = new DateTime(current.Year, current.Month, day);
				day++;
			}
		}
	}
}
