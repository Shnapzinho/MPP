namespace Project
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public bool IsBorrowed { get; set; }
		public int? BorrowedByMemberId { get; set; }
	}

	public class Member
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int BorrowedCount { get; set; }
		public const int MaxBooks = 3;
	}
}