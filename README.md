# QueuedDownloader
Just a testing application, which uses fake "FileDownloader" class to emulate downloading of files in queue
User can add urls into download queue (no checks for valid url of file to download in the current moment)
Each added url generates Task, each Task except the very first is added as continuation to previous
First-added task are started manually, other are started when previous task had completed its work
