from time import sleep
import asyncio

async def test():
    print("test Before sleep")
    sleep(3)
    print("test After sleep")
    return 20

async def test2():
    print("test2 Before sleep")
    sleep(1)
    print("test2 After sleep")
    return 4

async def control():
    print("Start control")
    results = []
    results.append(await test())
    results.append(await test2())
    print("End control")
    return results

async def control2():
    print("Start control")
    results = []
    results.append(await test2())
    print("End control")
    return results

async def main():
    print("Start main")
    tasks = []
    tasks.append(asyncio.create_task(control()))
    tasks.append(asyncio.create_task(control2()))
    print("End main", await asyncio.gather(*tasks))

if __name__ == "__main__":
    loop = asyncio.get_event_loop()
    res = loop.run_until_complete(main())
    print("End ", res)