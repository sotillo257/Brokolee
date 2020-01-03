from bs4 import BeautifulSoup
import requests, os
import pandas as pd
from selenium import webdriver

#os.chdir('C:\webdrivers')

#Enter User-agent:
chrome_path = os.path.join(os.path.dirname(__file__), 'driver', 'chromedriver')
header = {'User-agent' : 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36'}
options = webdriver.ChromeOptions();
options.add_argument("--start-maximized")

#Print Tm: Success

#team = [i.text for i in soup.find_all('td', {'data-stat': 'team_ID'})]
#print(team)

#Print G: Success

#games = [i.text for i in soup.find_all('td', {'data-stat': 'G'})]
#print(games)

#Print pythWL: Success

#py = [i.text for i in soup.find_all('td', {'data-stat': 'record_pythag'})]
#print(py)

#Print Tm, G: Success

#team = [i.text for i in soup.find_all('td', {'data-stat': 'team_ID'})]
#games = [i.text for i in soup.find_all('td', {'data-stat': 'G'})]
#my_dict2 = dict(zip(team, games))
#print(my_dict2)

#Trying to print out Tm, G, pythWL
class TeamAlgo():
    def __init__(self):
        self.driver = webdriver.Chrome(executable_path=chrome_path, options=options)
        self.driver.get('https://www.baseball-reference.com/leagues/MLB-standings.shtml')

        self.soup = BeautifulSoup(self.driver.page_source, 'html.parser')
        self.driver.quit()

    def parse_data(self):
        columns = ('Team', 'G', 'Pythag')
        df1 = pd.DataFrame(columns=columns)
        a = [i.text for i in self.soup.find_all('td', {'data-stat': 'team_ID'})]
        b = [i.text for i in self.soup.find_all('td', {'data-stat': 'G'})]
        c = [i.text for i in self.soup.find_all('td', {'data-stat': 'record_pythag'})]

        new_dict = {i:[j, k] for i, j, k in zip(a, b, c)}
        data_list = []

        i = 0
        print(new_dict)
        for key, value in new_dict.items():
            str_list = str(value[1]).split('-')
            data_item = {"team": key, "g": value[0], "pywins": str_list[0]}
            data_list.append(data_item)
            df1.loc[i] = (key, value[0], str_list[0])
            i = i + 1
        return {"data": data_list, "pandas": df1}