CS=gmcs
CSFLAGS=-unsafe -define:MONO
subdirs := . Gumps Items Menus Diagnostics Persistence Targeting Network ContextMenus
SRCS=$(foreach subdir,$(subdirs),$(wildcard $(subdir)/*.cs))

.PHONY += all

all: RunUO.exe

RunUO.exe: $(SRCS)
	$(CS) $(CSFLAGS) $(SRCS) -out:$@
