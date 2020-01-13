Namespace 基础

    ''' <summary>
    ''' 类型判断、转换、选择、比较的模块
    ''' </summary>
    Public Module 类型

        Private Function Oneof(t As Type, ParamArray b() As Type) As Boolean
            For Each i As Type In b
                If t = i Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断这个类型是否是整数类型 Byte Integer UInteger Long ULong Short UShort Decimal 
        ''' 并不判断其内容是否真的是整数
        ''' </summary>
        Public Function 是整数类型(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return Oneof(t,
                         GetType(Integer),
                         GetType(Long),
                         GetType(Byte),
                         GetType(UInteger),
                         GetType(ULong),
                         GetType(Short),
                         GetType(UShort),
                         GetType(Decimal))
        End Function

        ''' <summary>
        ''' 判断这个类型是否是小数类型 Single Double
        ''' 并不判断其内容是否真的是小数
        ''' </summary>
        Public Function 是小数(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return Oneof(t,
                         GetType(Single),
                         GetType(Double))
        End Function

        ''' <summary>
        ''' 判断这个类型是否是 Byte Integer UInteger Long ULong Short UShort Decimal Single Double
        ''' </summary>
        Public Function 是数字(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return 是整数类型(t) OrElse 是小数(t)
        End Function

        ''' <summary>
        ''' 判断这个类型是否是字符串 String Char
        ''' </summary>
        Public Function 是字符串(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return Oneof(t,
                         GetType(String),
                         GetType(Char))
        End Function

        ''' <summary>
        ''' 判断这个类型是否是 ICollection, IList, IDictionary, IEnumerable 的一个子类
        ''' </summary>
        Public Function 是集合(t As Type) As Boolean
            If IsNothing(t) Then Return False
            For Each i As Type In t.GetInterfaces()
                If Oneof(i,
                         GetType(ICollection),
                         GetType(IEnumerable),
                         GetType(IList),
                         GetType(IDictionary)) Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断t类型是否继承自parent类型
        ''' </summary>
        Public Function 是同一继承类(t As Type, parent As Type) As Boolean
            If IsNothing(t) OrElse IsNothing(parent) Then Return False
            If t = parent Then Return True
            Do While True
                If IsNothing(t.BaseType) Then Exit Do
                t = t.BaseType
                If t = parent Then Return True
            Loop
            Return False
        End Function

        ''' <summary>
        ''' 判断t类型是否是控件
        ''' </summary>
        Public Function 是控件(t As Type) As Boolean
            Return 是同一继承类(t, GetType(Control))
        End Function

        ''' <summary>
        ''' 判断这个类型是否有Length属性
        ''' </summary>
        Public Function 有Length(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return Not IsNothing(t.GetProperty("Length"))
        End Function

        ''' <summary>
        ''' 判断这个类型是否有Count属性
        ''' </summary>
        Public Function 有Count(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return Not IsNothing(t.GetProperty("Count"))
        End Function

        ''' <summary>
        ''' 判断这个类型是否有Count属性
        ''' </summary>
        Public Function 有LengthCount(t As Type) As Boolean
            If IsNothing(t) Then Return False
            Return 有Length(t) OrElse 有Count(t)
        End Function

        ''' <summary>
        ''' 判断这个对象是否为空
        ''' 数字判断是否为0
        ''' 有Length或者Count就判断是否为0
        ''' 是颜色判断是否为 Color.Empty
        ''' 是控件就判断 IsHandleCreated = False
        ''' </summary>
        Public Function 为空(对象 As Object) As Boolean
            If IsNothing(对象) Then Return True
            Dim t As Type = 对象.GetType
            Try
                If 是同一继承类(t, GetType(Stream)) Then Return False
                If 是数字(t) Then Return 对象 = 0
                If 有Count(t) Then Return 对象.Count = 0
                If 有Length(t) Then Return 对象.Length = 0
                If t = GetType(Color) Then Return 对象 = Color.Empty
            Catch ex As Exception
                出错(ex, t)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' 判断这些对象中是否有一个为空
        ''' </summary>
        Public Function 为空(ParamArray 对象() As Object) As Boolean
            For Each i As Object In 对象
                If 为空(i) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断这些对象中是否全部为空
        ''' </summary>
        Public Function 为空全部(ParamArray 对象() As Object) As Boolean
            For Each i As Object In 对象
                If 为空(i) = False Then Return False
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断这个对象是否非空
        ''' </summary>
        Public Function 非空(对象 As Object) As Boolean
            Return Not 为空(对象)
        End Function

        ''' <summary>
        ''' 判断这些对象中是否有一个非空
        ''' </summary>
        Public Function 非空(ParamArray 对象() As Object) As Boolean
            For Each i As Object In 对象
                If 非空(i) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断这些对象中是否全部非空
        ''' </summary>
        Public Function 非空全部(ParamArray 对象() As Object) As Boolean
            For Each i As Object In 对象
                If 非空(i) = False Then Return False
            Next
            Return True
        End Function

        ''' <summary>
        ''' 比较两个东西之间是否相等的办法
        ''' </summary>
        Public Enum 比较方法

            ''' <summary>
            ''' 直接使用 = 来比较
            ''' </summary>
            等于

            ''' <summary>
            ''' 只比较两个对象的 HashCode 是否相等
            ''' </summary>
            HashCode

            ''' <summary>
            ''' 只比较对象的 Tostring 是否相等
            ''' </summary>
            Tostring

            ''' <summary>
            ''' 使用 Equals 来比较
            ''' </summary>
            Equals

            ''' <summary>
            ''' 只比较两个对象的 Type 是否相等
            ''' </summary>
            Type

            ''' <summary>
            ''' 比较两个对象是否有相同的基础类或接口
            ''' </summary>
            BaseType

            ''' <summary>
            ''' 使用 Is 关键字来比较，判断两个对象是否引用自同一对象
            ''' </summary>
            [Is]

        End Enum

        ''' <summary>
        ''' 用指定的比较方法来比较A和B两个对象是否相等，如果比较出错，只返回 A.Equals(B)
        ''' </summary>
        Public Function 比较(A As Object, B As Object, 方法 As 比较方法) As Boolean
            Try
                Select Case 方法
                    Case 比较方法.等于
                        Return A = B
                    Case 比较方法.Tostring
                        Return A.ToString = B.ToString
                    Case 比较方法.Is
                        Return A Is B
                    Case 比较方法.Type
                        Return A.GetType = B.GetType
                    Case 比较方法.HashCode
                        Return A.GetHashCode = B.GetHashCode
                    Case 比较方法.BaseType
                        If A.GetType.BaseType <> GetType(Object) AndAlso A.GetType.BaseType = B.GetType Then Return True
                        If B.GetType.BaseType <> GetType(Object) AndAlso B.GetType.BaseType = A.GetType Then Return True
                        For Each i As Type In A.GetType.GetInterfaces
                            For Each i2 As Type In B.GetType.GetInterfaces
                                If i = i2 Then Return True
                            Next
                        Next
                        Return False
                End Select
                Return False
            Catch ex As Exception
                出错(ex)
            End Try
            Return A.Equals(B)
        End Function

        ''' <summary>
        ''' 用指定的办法来判断这个物品是否在列表内
        ''' </summary>
        Public Function 在列表(列表 As IList, 物品 As Object, 比较方法 As 比较方法) As Boolean
            If Not (IsNothing(列表) OrElse 列表.Count < 1 OrElse IsNothing(物品)) Then
                For Each i As Object In 列表
                    If 比较(i, 物品, 比较方法) Then Return True
                Next
            End If
            Return False
        End Function

        ''' <summary>
        ''' 针对list进行去重
        ''' </summary>
        Public Sub 列表去重(列表 As IList, 比较方法 As 比较方法)
            If 非空(列表) Then
                Dim m As New List(Of Object), i As Object
                For Each i In 列表
                    If IsNothing(i) = False AndAlso 在列表(m, i, 比较方法) = False Then m.Add(i)
                Next
                列表.Clear()
                For Each i In m
                    列表.Add(i)
                Next
            End If
        End Sub

        ''' <summary>
        ''' 判断寻找的对象是否=内容当中的一个
        ''' </summary>
        Public Function 是当中一个(寻找 As Object, ParamArray 内容() As Object) As Boolean
            If IsNothing(寻找) Then Return False
            For Each i As Object In 内容
                If 比较(i, 寻找, 比较方法.等于) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' 判断寻找的对象是否是内容当中的一个，并使用对应的比较办法
        ''' </summary>
        Public Function 是当中一个(比较办法 As 比较方法, 寻找 As Object, ParamArray 内容() As Object) As Boolean
            If IsNothing(寻找) Then Return False
            For Each i As Object In 内容
                If 比较(i, 寻找, 比较办法) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' 互换AB两个对象
        ''' </summary>
        Public Sub 互换(ByRef A As Object, ByRef B As Object)
            Dim C As Object = B
            B = A
            A = C
        End Sub

        ''' <summary>
        ''' 反转A
        ''' </summary>
        Public Function 反转(ByRef A As Boolean) As Boolean
            A = Not A
            Return A
        End Function

        ''' <summary>
        ''' 如果对象等于A，那么把对象换成B
        ''' </summary>
        Public Function 反转(ByRef 对象 As Object, A As Object, B As Object) As Object
            If 非空(对象) Then
                If 对象 = A Then
                    对象 = B
                ElseIf 对象 = B Then
                    对象 = A
                End If
            End If
            Return 对象
        End Function

        ''' <summary>
        '''  If A 小于 最小值 那么 A = 最小值
        ''' </summary>
        Public Function 设最小值(ByRef A As Object, 最小值 As Object) As Object
            If A < 最小值 Then A = 最小值
            Return A
        End Function

        ''' <summary>
        '''  If A 大于 最小值 那么 A = 最小值
        ''' </summary>
        Public Function 设最大值(ByRef A As Object, 最大值 As Object) As Object
            If A > 最大值 Then A = 最大值
            Return A
        End Function

    End Module

End Namespace