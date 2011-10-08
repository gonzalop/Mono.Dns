MCS=$(if $(V),,@echo "MCS	$@";) gmcs 
RM_F=rm -f 
SOURCES= \
	Mono.Dns/DnsClass.cs \
	Mono.Dns/DnsHeader.cs \
	Mono.Dns/DnsOpCode.cs \
	Mono.Dns/DnsPacket.cs \
	Mono.Dns/DnsQClass.cs \
	Mono.Dns/DnsQType.cs \
	Mono.Dns/DnsQuery.cs \
	Mono.Dns/DnsQuestion.cs \
	Mono.Dns/DnsRCode.cs \
	Mono.Dns/DnsResourceRecordA.cs \
	Mono.Dns/DnsResourceRecordAAAA.cs \
	Mono.Dns/DnsResourceRecordCName.cs \
	Mono.Dns/DnsResourceRecordIPAddress.cs \
	Mono.Dns/DnsResourceRecordPTR.cs \
	Mono.Dns/DnsResourceRecord.cs \
	Mono.Dns/DnsResponse.cs \
	Mono.Dns/DnsType.cs \
	Mono.Dns/DnsUtil.cs \
	Mono.Dns/ResolverAsyncOperation.cs \
	Mono.Dns/ResolverError.cs \
	Mono.Dns/SimpleResolver.cs \
	Mono.Dns/SimpleResolverEventArgs.cs

DLLS= Mono.Dns.dll
EXES = resolver.exe \
	plainolddns.exe

all: $(DLLS) $(EXES)

Mono.Dns.dll: $(SOURCES)
	$(MCS) -target:library -out:$@ -debug $^

resolver.exe: test/resolver.cs Mono.Dns.dll
	$(MCS) -r:Mono.Dns.dll -out:$@ -debug $<

plainolddns.exe: test/plainolddns.cs
	$(MCS) -out:$@ -debug $^
clean:
	$(RM_F) $(DLLS) $(EXES) *.mdb

