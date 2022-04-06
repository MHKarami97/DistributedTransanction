from ast import arg
import os
import random
import subprocess
from time import sleep

iteration_count = 100
reset_factor = 2
reset_min = 1
wait_factor = 4
wait_min = 1

def kill(proc):
    subprocess.call(['taskkill', '/F', '/T', '/PID', str(proc.pid)])

def popen(path, exe):
    return subprocess.Popen(path + '/' + exe, shell=True, cwd=path, stdout=subprocess.DEVNULL, stderr= subprocess.DEVNULL)

def reset(proc, name):
    args = proc.args
    kill(proc)
    print("%s killed"%name)
    duration = random.random() * reset_factor + reset_min 
    print("wait for %d before start"%duration)
    sleep(duration)
    proc = popen(os.path.dirname(args), os.path.basename(args))
    print("%s started"%name)
    return proc

def reset_oms():
    global oms
    oms = reset(oms, "oms")

def reset_cas():
    global cas
    cas = reset(cas, "cas")

def reset_recovery_agent():
    global recovery_agent
    recovery_agent = reset(recovery_agent, "recover_agent")

actions = [ reset_cas, reset_oms, reset_recovery_agent]

try:
    oms = popen("D:/Repos/DistributedTransanction/Oms/bin/Debug/net6.0", "Oms.exe")
    cas = popen("D:/Repos/DistributedTransanction/Accounting/bin/Debug/net6.0", "Accounting.exe")
    recovery_agent = popen("D:/Repos/DistributedTransanction/RecoveryAgent/bin/Debug/net6.0", "RecoveryAgent.exe")

    for i in range(iteration_count):
        durtion = random.random() * wait_factor + wait_min
        print("wait for %d seconds"%durtion)
        wait_time = sleep(durtion)
        random.choice(actions)()

finally:
    kill(recovery_agent)
    kill(oms)
    kill(cas)