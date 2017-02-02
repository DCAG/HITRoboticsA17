#$query = "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub' AND TargetInstance.DeviceID LIKE '%VID_{0}`&PID_{1}%'" -f '0951','1666'
$query = "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort' AND TargetInstance.DeviceID LIKE '%VID_{0}`&PID_{1}%'" -f '0951','1666'
Register-WmiEvent -Query $query -SourceIdentifier insert 
Wait-Event -SourceIdentifier insert
Get-Event -SourceIdentifier insert | % SourceEventArgs | % NewEvent | % TargetInstance -ov res
Get-EventSubscriber -SourceIdentifier insert | unregister-event

Get-Event -SourceIdentifier insert | Remove-Event