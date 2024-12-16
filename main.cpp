// NOTE: it is recommended to use this even if you don't understand the following code.

#include <bits/stdc++.h>

using namespace std;
//vector<int> v;

/*
int suca(int i, int j){
    int x = v[i-1] & v[j-1];
    return x;
}
*/

int main() {
// uncomment the following lines if you want to read/write from files
    //ifstream cin("input.txt");
    //ofstream cout("output.txt");

    int N;
    cin >> N;

    vector<int> m(N+1000, 0);
    vector<set<int>> x(N+1000, set<int>());

    /*
    v.resize(N);
    for(int i = 0; i < N; i++){
        cin >> v[i];
        cout << v[i] << " ";
    }
    cout << endl;
    */

    for(int i = 1; i <= N; i++){
        for(int j = 1; j <= N; j++){
            if(x[i].count(j) == 1) continue;
            x[i].insert(j);
            x[j].insert(i);


            cout << "? " << i << ' ' << j << '\n';
            cout.flush();
            int ans = 0;
            cin >> ans;

            //int ans = suca(i, j);
            //cout << i << " " << j << " " << ans << " " << m[i] << " " << m[j] << endl;

            if(ans == -1) continue;
            ans = 1 << ans;
            if(m[i] != 0){
                if((m[i] & ans) == 0) m[i] += ans;
            }
            else{
                m[i] = ans;
            }


            if(m[j] != 0){
                if((m[j] & ans) == 0) m[j] += ans;
            }
            else{
                m[j] = ans;
            }

        }
    }

    cout << "! ";
    for(int i = 1; i <= N; i++)
        cout << m[i] << " ";
    cout << endl;

    // SAMPLE INTERACTION, REPLACE WITH YOUR CODE
/*
	cout << 1 << ' ' << 2 << '\n';
	cout.flush();
	int x;
	cin >> x;

    cout << "! "; // print the result
    for (int i = 0; i < N; i++)
		cout << i + 1 << ' ';
	cout << endl;
*/
    return 0;
}
