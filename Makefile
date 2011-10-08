MCS=$(if $(V),,@echo "MCS	$@";) gmcs 
RM_F=rm -f 
SOURCES= \
	Mono.Net.Dns/DnsClass.cs \
	Mono.Net.Dns/DnsHeader.cs \
	Mono.Net.Dns/DnsOpCode.cs \
	Mono.Net.Dns/DnsPacket.cs \
	Mono.Net.Dns/DnsQClass.cs \
	Mono.Net.Dns/DnsQType.cs \
	Mono.Net.Dns/DnsQuery.cs \
	Mono.Net.Dns/DnsQuestion.cs \
	Mono.Net.Dns/DnsRCode.cs \
	Mono.Net.Dns/DnsResourceRecordA.cs \
	Mono.Net.Dns/DnsResourceRecordAAAA.cs \
	Mono.Net.Dns/DnsResourceRecordCName.cs \
	Mono.Net.Dns/DnsResourceRecordIPAddress.cs \
	Mono.Net.Dns/DnsResourceRecordPTR.cs \
	Mono.Net.Dns/DnsResourceRecord.cs \
	Mono.Net.Dns/DnsResponse.cs \
	Mono.Net.Dns/DnsType.cs \
	Mono.Net.Dns/DnsUtil.cs \
	Mono.Net.Dns/ResolverAsyncOperation.cs \
	Mono.Net.Dns/ResolverError.cs \
	Mono.Net.Dns/SimpleResolver.cs \
	Mono.Net.Dns/SimpleResolverEventArgs.cs

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

