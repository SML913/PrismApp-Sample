   M S B A M L   `   `   `  ����  UI     UI.Views.CompanyDetailView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      -   5          >�Name$  CompanyDetail��5      !  Prism.Mvvm.ViewModelLocator   AutoWireViewModel . .��5
      � 
�����w� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $	��150 �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��2��6   $��Company Name��6$   $��5 5 5 0q�6;   $	��400��6L   �� 5   
   $��1��6   $��0��6    $��2��60   $	��400��6D   $��10,10,10,10q�5   a   ��6   �� *Company.Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6a   �� 5   
   $��Add Employee��6   $��2��6(   $��25,17,10,18q�66   $	��125��6K   $��35��6W   $��1��5   8   ��6   �� *AddEmployeeCommand+68   �� 5   
   $��2��6   $��0��6!   $��10,10,10,10q�62    �� DisplayMemberPath$ DisplayMember��5      $��35��65   ��5      �� *AvailableEmployees+ �� SelectedItem 5       �� *SelectedEmployeeToAdd+$ TwoWay=�&   System.Windows.Controls.DataGrid  5R   
   $��3��6   $��0��6!   $��2��61      AutoGenerateColumns .  	   CanUserAddRows	 .  
   RowHeaderWidth$
 0Q�5T   G   $	��400��6Z   $��10,10,10,10q�5U      ��5R   E   �� *	Employees+ 5S      �� *SelectedEmployee+$ TwoWay=�$ PropertyChanged=�   Columns 5W      0  *System.Windows.Controls.DataGridTextColumn  5X      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Employees��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 68      DataGridColumnStyle1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  5Y   &      Binding 5Z   &   �� *DisplayMember+4  .System.Windows.Controls.DataGridTemplateColumn  5[      $ Delete��6)      CellTemplate 5]      �� 5^      .R�5_      �� $��X��6%   
�������$
��Bold"�6D   $��20(�6V     ChromelessButtonStyle#��[ 5b   #   ��5`   %   ��  �� RelativeSource ��  �� AncestorType"   �� Path$ DataContext.DeleteCommand�5b   #   5c      5f      5h      �� 5j   
   $��4��6   $��2��6"   $��0��66    �� Orientation$ 
Horizontal=�6F   .+�5k      �� $
��Save��6   $��80��6$   $��5 5 5 5q�6/   ��6@   �� *SaveCommand+�� 5l      $��Close��6   $��80��6%   $��5 5 5 5q�60   ��6A   �� *CloseCommand+5m      5o      5p      