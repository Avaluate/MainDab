   M S B A M L   `   `   `  ����L  HMainDabDownloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MainDab_BootstrapperMainDab_Bootstrapper  5   	   %    MainDab_Bootstrapper.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5   	   .local"clr-namespace:MainDab_Bootstrapper  5   	   -   5          � Title$  
MainWindow��5	   	   $��326.533��6   $��564.8��6-    � 
ResizeMode$ NoResize=�6;    � WindowStyle$
 None=�6Q    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6�   ��6d   Z� ��6z   Z�  � 	Resources 5         NoHighlightBut �� 
TargetType ��Name J� SnapsToDevicePixels	 ��Name
 �� ContentStringFormat �� ContentStringFormat �� Triggers P� Property P� Value �� 
TargetName �� Value �� Property  TextBox.Static.Border �� Color  TextBox.MouseOver.Border  TextBox.Focus.Border �� 
TargetType-  'System.Windows.Input.KeyboardNavigation ��Name �� Triggers ]� 
Conditions �� Property �� Value  RichTextBoxStyle1 �� 	Resources �� BasedOn �� ResourceKey��%�
  &	        &	     &	 )    &	 L    '�� o    7  '�� �    &	 �    7��0�� 5d      " ��6!   1�� 5   
   " ��61   .R�5      �� $ border��6   #��z��6%   #��z��6Q   #��z��6�    . ._�5      �� $	 contentPresenter��6#   #��z��6=   #��z��6q   # z
 6�   |�.  #��z��6�   #��z��6&  ��. # z 6d  #��z��6�  5       5      P� 5       �@��IsDefaulted . . �5      �� $ border��64   # � ��6H    � ��5      P� 5       �@d�	IsChecked$
 TrueY�6<   . �5      �� $ border��63    � �� �	#4C4646466G   �� 5      $ border��64    � �� �	#4C4646466H   5      P� 5       � {� .  . �5      �� $ border��63    � �� �	#4C4646466G   �� 5      $ border��64    � �� �	#4C4646466H   �� 5       � �� �	#4C46464663   5      5       �� 5!   
   $ 	#FFABAdB3��68   �� 5"   
   $ 	#FF7EB4EA��6;   �� 5#   
   $ 	#FF569DE5��67   �� 5$   
   " ��6-   .�5%      ��  � ��# � ��6+   �� 5&       � ��8   6,   �� 5'       � ��# � ��6+   �� 5(       � ��$ 1q�60   �� 5)       �@ TabNavigation$
 None=�6A   �� 5*       � ��$
 Left=�6;   �� 5+       �@�FocusVisualStyle 61   Z� �� 5,       �@J�	AllowDrop . 6*   �� 5.       �@��IsFlicksEnabled .  67   �� 5/       � �� 50      �� 51      " ��6&   .R�52      �� $ border��6!   #��z��61   #��z��6]   #��z��6�    . ._�53      �� $ PART_ContentHost��6+   |�.  $��Hidden=�6W   $��Hidden=�6~   54      56      58       59      ]� 5:       5;      �� 5<      + �@��#IsInactiveSelectionHighlightEnabled . 6S   �� 5=       �@��IsSelectionActive .  6A   ./�5?      ��  �@��SelectionBrush# � /�67   5@      5B      �� 5C   
   " ��6+   .�5D      ��  � ��# � ��6+   �� 5E       �@��TextDecorations$ 	Underline{�60    5F      P� 5G       �@��IsMouseOver . . �5H      ��  � ��
 �  ��63   5I      P� 5J       �@��	IsEnabled .  . �5K      ��  � ��# � ��63   5L      P� 5M       �@��	IsEnabled . . �5N      ��  �@�Cursor$
 Hand��6/   5O      5Q      �� 5R   
   " ��6*    5S      ���� 5T      '/�       " /�66   .�5U      ��  �@�OverridesDefaultStyle . 6>   5V      �� 5W      '��       8   " ��6a    5X      P� 5Y       �@��IsMouseOver . . �5Z      ��  � ��
 ��  �6;   5[      P� 5\       �@��	IsEnabled .  . �5]      ��  � ��# � ��6;   5^      5`      5a      .�5b      ��  � ��$ 10��6)    5c      2 5f      5g      .��5h      � -   5         �Name$ MainGrid��5h      $
��Left=�6   $	��326��68   $��
0,0,-0.2,0q�6E   $	��Top=�6Y   $	��565��6q   .+�5i   
   �� -   5        $ 
MainBorder��5i       �� CornerRadius$ 4��6%   ���	#FF797979$��1q�6N   $
��Left=�6b   $	��326��6}   $	��Top=�6�   $	��565��6�   ���	#FF1414146�   �� 5j   
   -   5         ��Name$ 
DownloadBG��5j       �� BitmapScalingMode$ HighQuality=�6$   $
��Left=�6R   $	��325��6m   $��-2,0,0,0q�6z   $	��Top=�6�   $	��565��6�   $2��,Untitled drawing - 2023-01-09T193406.617.png��6�     J� Opacity$  0Q�6�   $��UniformToFill=�6�   � 5k   
   -   5        $ Startup��5k      $
��Left=�6    $	��326��6;   $	��Top=�6H   $	��565��6`   .+�5l      �� -   5        $ MainDabIcon��5l      ! �� EdgeMode$! Aliased=�6*   $
��Left=�6K   $	��310��6f   $��
128,14,0,0q�6s   $	��Top=�6�   $	��303��6�   $��MainDabIcon.png��6�   $	  0.8Q�6�   " J� RenderTransformOrigin$" 0.5,0.25&�6�   $
��Fill=�6�   z�5m      V� 5n      .~�5o      �� �� 5p      �� 5q      T� 5r      # T� Y$# 0Q�6-   5s      5u      �� 5v      -   5        $ ��Name$$ MainDabTitle��5v      $��Center=�6*   $��Welcome to MainDab��6N   $
��Left=�6k   $��42��6�   $��	0,203,0,0q�6�   $	��Top=�6�   $	��565��6�   ���	#FFC5C5C5$9��3/MainDab Bootstrapper;component/#Chakra Petch Light)�6�   $��25(�6!  5w      � 5x   
   -   5        $ DownloadMainDab��5x      $��1,1,1,0q�6(   $y�Hidden=�69   .+�5y      �� -	   5        $$ InstallUpdateText��5y      $��Center=�6/   $��MainDab Downloader��6S   $
��Left=�6p   $��42��6�   $��0,139,-2.2,0q�6�   $	��Top=�6�   $	��565��6�   ���	#FFC5C5C5$9��3/MainDab Bootstrapper;component/#Chakra Petch Light)�6�   $��25(�6)  $" 0.5,1&�67  �� 5z      -
   5        $$ LatestUpdate��5z      $��Center=�6*   $��MainDab version here��6N   $
��Left=�6m   $��44��6�   $��-2,218,-0.2,0q�6�   $	��Top=�6�   $	��565��6�   ���	#FF918F8F$��Calibri Light)�6�   $��15(�6  $" 0.5,1&�6  �� 5{      -   5        % ��Name$% InstallButton��5{      #���   6,   $��Install MainDab��6Y   $
��Left=�6s   $��216,182,0,0q�6�   $	��Top=�6�   $	��132��6�   ���	#03FFFFFF
�������$
  0.75Q�6�   $��34��6   $" 0.5,1&�6  6J  5|      � 5}   
   -   5        $ InstallMainDab��5}      $
��Left=�6'   $	��324��6B   $��1,1,0,0q�6O   $	��Top=�6`   $	��563��6x   $y�Hidden=�6�   & � RowDefinitions& 5~      �� 5      $��0* �6    �� 5�      .+�5�      �� -   5        $ Gif1Completed��5�      $ HighQuality=�6+   $! Aliased=�6Y   $
��Left=�6z   $��20��6�   $��246,119,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $" 0.316,-0.333&�6�   $	  0.8Q�6  $��
accept.png��6  $y�Hidden=�61  �� 5�      -   5        $ Gif2Completed��5�      $ HighQuality=�6+   $! Aliased=�6Y   $
��Left=�6z   $��20��6�   $��246,148,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $" 0.316,-0.333&�6�   $	  0.8Q�6  $��
accept.png��6  $y�Hidden=�61  �� 5�      -   5        $ Gif3Completed��5�      $ HighQuality=�6+   $! Aliased=�6Y   $
��Left=�6z   $��20��6�   $��246,177,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $" 0.316,-0.333&�6�   $	  0.8Q�6  $��
accept.png��6  $y�Hidden=�61  �� 5�      -   5        $ Gif4Completed��5�      $ HighQuality=�6+   $! Aliased=�6Y   $
��Left=�6z   $��20��6�   $��246,206,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $" 0.316,-0.333&�6�   $	  0.8Q�6  $��
accept.png��6  $y�Hidden=�61  �� 5�      -   5        $
 Gif2��5�      $ HighQuality=�6"   $! Aliased=�6P   $
��Left=�6q   $��20��6�   $��246,148,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   "  WpfAnimatedGif.ImageBehavior'   AnimatedSource$' Rolling-1s-800px.gif��6�   $" 0.316,-0.333&�6  $	  0.8Q�6>  �� 5�      -   5        $$ InstallUpdateText_Copy��5�      $��Center=�64   $��Download Progress��6X   $
��Left=�6t   $��42��6�   $��0,1,0,0q�6�   $	��Top=�6�   $	��563��6�   ���	#FFC5C5C5$��Calibri Light)�6�   $��25(�6  $" 0.5,1&�6  $��2��6.  �� 5�      -   5        $$ DeletingFilesLabel��5�      $��Deleting files��60   $
��Left=�6I   $��42��6d   $��
35,140,0,0q�6p   $	��Top=�6�   $	��138��6�   ���	#FFC5C5C5$��Calibri Light)�6�   $��20(�6�   $" 0.5,1&�6�   $��2��6  �� 5�      -   5        $
 Gif3��5�      $ HighQuality=�6"   $! Aliased=�6P   $
��Left=�6q   $��20��6�   $��246,177,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $' Rolling-1s-800px.gif��6�   $" 0.316,-0.333&�6  $	  0.8Q�6>  �� 5�      -   5        $$ CreatingFoldersLabel��5�      $��Creating folders��62   $
��Left=�6M   $��42��6h   $��
35,169,0,0q�6t   $	��Top=�6�   $	��138��6�   ���	#FFC5C5C5$��Calibri Light)�6�   $��20(�6�   $" 0.5,1&�6�   $��2��6
  �� 5�      -   5        $
 Gif4��5�      $ HighQuality=�6"   $! Aliased=�6P   $
��Left=�6q   $��20��6�   $��246,206,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $' Rolling-1s-800px.gif��6�   $" 0.316,-0.333&�6  $	  0.8Q�6>  �� 5�      -   5        $$ DownloadingMainDabLabel��5�      $��Downloading MainDab��65   $
��Left=�6S   $��42��6n   $��
35,198,0,0q�6z   $	��Top=�6�   $	��195��6�   ���	#FFC5C5C5$��Calibri Light)�6�   $��20(�6�   $" 0.5,1&�6�   $��2��6  �� 5�      -   5        $
 Gif1��5�      $ HighQuality=�6"   $! Aliased=�6P   $
��Left=�6q   $��20��6�   $��246,119,0,0q�6�   $��2��6�   $	��Top=�6�   $��21��6�   $' Rolling-1s-800px.gif��6�   $" 0.316,-0.333&�6  $	  0.8Q�6>  �� 5�      -   5        $$ CheckingRequirementsLabel��5�      $��Checking requirements��67   $
��Left=�6W   $��42��6r   $��
35,111,0,0q�6~   $	��Top=�6�   $	��202��6�   ���	#FFC5C5C5$��Calibri Light)�6�   $��20(�6�   $" 0.5,1&�6�   $��2��6  � 5�      -   5        $ RequirementCheck��5�      $
��Left=�6-   $	��159��6H   $��
301,95,0,0q�6U   $	��Top=�6i   $	��242��6�   $��2��6�   .+�5�      �� -   5        ( ��Name$( CheckRequireTitle��5�      $��Center=�69   $
��Left=�6]   $��26��6x   $��0,3,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFEAEAEA$��19(�6#  $��Calibri)�61  ) �� 
IsReadOnly) . #���  6�  {�.  ��6�   Z� ��6  Z� * �� SelectionBrush* 6X  Z� + �� SelectionTextBrush+ 6r  Z� . �5�      /� , /� TextAlignment$, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $�Checking Requirements��6"   5�      5�      5�      �� 5�      -   5        $( CheckRequireText��5�      $��Center=�68   $
��Left=�6\   #���  6w   $��46��6�   $��0,29,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFC5C5C5$��16(�6O  $��Calibri Light)�6]  ) . {�.  ��6�   Z� ��68  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $?�9MainDab needs to check whether MainDab can be downloaded.��6"   5�      5�      5�      �� 5�      $ HighQuality=�6   $
��Left=�6G   $��78��6b   $��	73,72,0,0q�6n   $	��Top=�6�   $��99��6�   $��list.png��6�   $
  0.55Q�6�   5�      � 5�      -   5        $ DeletingFiles��5�      $
��Left=�6*   $	��159��6E   $��301,100,0,0q�6R   $	��Top=�6g   $	��242��6   $��2��6�   $y�Hidden=�6�   .+�5�      �� -   5        $( DeletingFilesTItle��5�      #���  6:   $��Center=�6f   $
��Left=�6�   $��26��6�   $��0,3,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFEAEAEA$��19(�6P  $��Calibri)�6^  ) . {�.  ��6�   Z� ��69  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $�Deleting files��6"   5�      5�      5�      �� 5�      -   5        $( DeletingFilesText��5�      #���  69   $��Center=�6e   $
��Left=�6�   $��46��6�   $��0,29,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFC5C5C5$��16(�6P  $��Calibri Light)�6^  ) . {�.  ��6�   Z� ��69  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $D�>We are now deleting old files, alongside DLL files not needed.��6"   5�      5�      5�      �� 5�      $ HighQuality=�6   $
��Left=�6G   $��63��6b   $��	90,78,0,0q�6n   $	��Top=�6�   $��78��6�   $��bin.png��6�   $
  0.55Q�6�   5�      � 5�      -    5        $ CreatingFolders��5�      $
��Left=�6,   $	��168��6G   $��
301,91,0,0q�6T   $	��Top=�6h   $	��242��6�   $��2��6�   $y�Hidden=�6�   .+�5�      �� -!   5        $( CreatingFoldersTitle��5�      #���  6<   $��Center=�6h   $
��Left=�6�   $��26��6�   $��0,3,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFEAEAEA$��19(�6R  $��Calibri)�6`  ) . {�.  ��6�   Z� ��6;  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $�Creating folders��6"   5�      5�      5�      �� 5�      -"   5        $( CreatingFilesText��5�      #���  69   $��Center=�6e   $
��Left=�6�   $��64��6�   $��0,29,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFC5C5C5$��16(�6P  $��Calibri Light)�6^  ) . {�.  ��6�   Z� ��69  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $U�OIf this is your first time using MainDab, a folder called MainDab will be made.��6"   5�      5�      5�      �� 5�      $ HighQuality=�6   $
��Left=�6G   $��73��6b   $��
84,92,0,-6q�6n   $	��Top=�6�   $	��101��6�   $��icons8-folder-250.png��6�   $
  0.55Q�6�   $" 0.508,0.379&�6�   5�      � 5�      -#   5        $ DownloadingMainDab��5�      $
��Left=�6/   $	��110��6J   $��301,117,0,0q�6W   $	��Top=�6l   $	��242��6�   $��2��6�   $y�Hidden=�6�   .+�5�      �� -$   5        $( DownloadingTitle��5�      #���  68   $��Center=�6d   $
��Left=�6�   $��26��6�   $��0,3,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFEAEAEA$��19(�6N  $��Calibri)�6\  ) . {�.  ��6�   Z� ��67  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $�Downloading MainDab��6"   5�      5�      5�      �� 5�      -%   5        $( DownloadingText��5�      #���  67   $��Center=�6c   $
��Left=�6�   $��64��6�   $��0,29,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFC5C5C5$��16(�6N  $��Calibri Light)�6\  ) . {�.  ��6�   Z� ��67  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $M�GIt shouldn't take long to download MainDab, as it's a single .exe file.��6"   5�      5�      5�      � 5�      -&   5        - �Name$- DownloadBar��5�      $
��Left=�63   $��20��6N   $��	10,79,0,0q�6Z   $	��Top=�6m   $	��222��6�   ���	#7FABABAB���	#FFE5E5E5$	��0.5q�6�   . � Value$. -1Q�6�   ��6�   Z� 6�   5�      � 5�      -'   5        $ ContinueToMainDab��5�      $
��Left=�6.   $	��164��6I   $��
301,90,0,0q�6V   $	��Top=�6j   $	��242��6�   $��2��6�   $y�Hidden=�6�   .+�5�      �� -(   5        $( ContinueToMainDabTitle��5�      #���  6>   $��Center=�6j   $
��Left=�6�   $��26��6�   $��0,3,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFEAEAEA$��19(�6T  $��Calibri)�6b  ) . {�.  ��6�   Z� ��6=  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $�Download completed��6"   5�      5�      5�      �� 5�      -)   5        $( ContinueToMainDabText��5�      #���  6=   $��Center=�6i   $
��Left=�6�   $��47��6�   $��0,29,0,0q�6�   $	��Top=�6�   $	��235��6�   ���	#FFC5C5C5$��16(�6T  $��Calibri Light)�6b  ) . {�.  ��6�   Z� ��6=  Z� * 6�  Z� + 6�  Z� . �5�      /� $, Center=�6#   .T�5�      J� .*�6$   .*�5�      �� $D�>MainDab has been successfully downloaded. Feel free to use it.��6"   5�      5�      5�      �� 5�      $ HighQuality=�6   $
��Left=�6F   $	��106��6a   $��64,67,0,-14q�6n   $	��Top=�6�   $	��123��6�   $��download (1).png��6�   $	  0.8Q�6�   5�      5�      �� 5�   
   -*   5        $ JoinDiscord��5�      $ HighQuality=�6%   $
��Left=�6S   $��16��6n   $��	7,306,0,0q�6z   $	��Top=�6�   $��20��6�   $#��5761437_discord_logo_icon.png��6�   $	  0.5Q�6�   $y�Hidden=�6�   �� 5�   
   -+   5        $$ JoinOurDiscord��5�      $��maindab.org/discord��6)   $
��Left=�6G   $��24��6b   $��
22,302,0,0q�6n   $	��Top=�6�   $	��166��6�   ���	#FF938E8E$��Calibri Light)�6�   $" 0.5,1&�6�   $��Italic$�6�   $y�Hidden=�6	  5�      5�      