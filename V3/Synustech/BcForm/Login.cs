using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test;

namespace Synustech.BcForm
{       
    public partial class Login : Form
    {
     
        public delMenuBtnVisible buttonVisible;
        public delLoginlblIdChanged lblIdChange;


        public Login(delMenuBtnVisible buttonVisible, delLoginlblIdChanged lblIdChange)
        {
            InitializeComponent();
 
            this.buttonVisible = buttonVisible;
            this.lblIdChange = lblIdChange;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2); // 화면 중앙에 위치
            if (bcComoboBox1.Items.Count > 0)
            {
                bcComoboBox1.SelectedItem = "User"; // "User" 선택
            }

            this.KeyPreview = true; // 키 입력을 먼저 받도록 설정
            this.KeyDown += Login_KeyDown; // KeyDown 이벤트 핸들러 추가
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (VKeyboard.keyboardPs != null && !VKeyboard.keyboardPs.HasExited)
            {
                VKeyboard.hideKeyboard();
            }
            Close();
        }

        private void btn_loginCheck_Click(object sender, EventArgs e)
        {
            if (bcComoboBox1.SelectedItem != null)
            {
                string selectedItem = bcComoboBox1.SelectedItem.ToString();

                if (selectedItem == "User")
                {
                    buttonVisible?.Invoke("btn_Motion", false);
                    buttonVisible?.Invoke("btn_Setting", false);
                    buttonVisible?.Invoke("btn_Teaching", false);
                }
                else if (selectedItem == "Admin")
                {
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("비밀번호를 입력해 주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // 비밀번호 미입력시 함수 종료
                    }
                    else if (textBox1.Text == "0000")
                    {
                        // Admin에 대한 권한이 주어짐
                        buttonVisible?.Invoke("btn_Motion", true);
                        buttonVisible?.Invoke("btn_Setting", false);
                        buttonVisible?.Invoke("btn_Teaching", true);
                    }
                    else
                    {
                        // 비밀번호가 틀림
                        MessageBox.Show("비밀번호가 틀렸습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 잘못된 경우 함수 종료
                    }
                }
                else if(selectedItem == "Maker")
                {
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        // 비밀번호를 입력하지 않은 경우
                        MessageBox.Show("비밀번호를 입력해 주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // 잘못된 경우 함수 종료
                    }
                    else if (textBox1.Text == "0618")
                    {
                        // Maker에 대한 권한이 주어짐
                        buttonVisible?.Invoke("btn_Motion", true);
                        buttonVisible?.Invoke("btn_Setting", true);
                        buttonVisible?.Invoke("btn_Teaching", true);
                    }
                    else
                    {
                        // 비밀번호가 틀림
                        MessageBox.Show("비밀번호가 틀렸습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 잘못된 경우 함수 종료
                    }
                }
                else
                {
                    MessageBox.Show("비밀번호가 틀렸습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // 잘못된 경우 함수 종료
                }
            
                lblIdChange?.Invoke(selectedItem);
            }
            if (VKeyboard.keyboardPs != null && !VKeyboard.keyboardPs.HasExited)
            {
                VKeyboard.hideKeyboard();
            }
            Close();
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_loginCheck_Click(sender, e); // Enter 키를 누르면 로그인 버튼 클릭 메서드 실행
            }
        }


        private void KeyBoardShow(object sender, EventArgs e)
        {
            //VKeyboard.showKeyboard();
        }
        
    }
}
