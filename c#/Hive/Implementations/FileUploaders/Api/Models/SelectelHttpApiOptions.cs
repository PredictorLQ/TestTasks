using System;

namespace FilesCleaner;

public sealed class SelectelHttpApiOptions
{
	public SelectelHttpApiOptions(
		String accountNumber,
		String payloadUser,
		String payloadPassword,
		String payloadContainer)
	{
		AccountNumber = accountNumber;
		PayloadUser = payloadUser;
		PayloadPassword = payloadPassword;
		PayloadContainer = payloadContainer;
	}

	public String AccountNumber { get; }
	public String PayloadUser { get; }
	public String PayloadPassword { get; }
	public String PayloadContainer { get; }
}