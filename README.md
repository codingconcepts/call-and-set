# call-and-set
Calls a command and sets environment variables based on what it returns.

## Build and publish

```
$ dotnet build
$ dotnet publish -r linux-x64
```

## Usage

```
$ dotnet build && ./bin/Debug/netcoreapp3.1/call-and-set.exe aws sts assume-role --role-arn "arn:aws:iam::123456789012:role/ci_cd" --role-session-name TempRole \
	--exp Credentials.AccessKeyId~TEMP_ENVVAR_AKI \
	--exp Credentials.SecretAccessKey~TEMP_ENVVAR_SAK \
	--exp Credentials.SessionToken~TEMP_ENVVAR_ST
```