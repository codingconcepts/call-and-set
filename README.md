# call-and-set
Calls a command and sets environment variables based on what it returns.
## Usage

```
$ call-and-set.exe aws sts assume-role --role-arn "arn:aws:iam::123456789012:role/ci_cd" --role-session-name TempRole \
	--env Credentials.AccessKeyId~TEMP_ENVVAR_AKI \
	--env Credentials.SecretAccessKey~TEMP_ENVVAR_SAK \
	--env Credentials.SessionToken~TEMP_ENVVAR_ST
```